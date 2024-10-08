import os
import sys

# Get the current script directory
script_dir = os.path.dirname(os.path.abspath(__file__))

# Construct paths for key directories (for the embedded Python environment)
lib_dir = os.path.join(script_dir, 'Lib')
site_packages_dir = os.path.join(lib_dir, 'site-packages')

# Add the necessary paths to sys.path for the embedded environment to function
sys.path.insert(0, lib_dir)
sys.path.insert(0, site_packages_dir)

import shutil
import argparse
import runpy
import importlib

# Function to clear module cache and release potential locks
def reset_module_cache(module_name):
    if module_name in sys.modules:
        print(f"Reloading module: {module_name} to release resources.")
        importlib.reload(sys.modules[module_name])

def run_model_builder(input_dir, output_dir, precision, execution, cache_dir=None):
    """
    Function to directly run the ONNX model builder by invoking the onnxruntime_genai module.
    """
    # Simulate the arguments for the module by replacing sys.argv
    sys.argv = [
        "builder",  # This represents the script name
        "-i", input_dir,
        "-o", output_dir,
        "-p", precision,
        "-e", execution
    ]
    
    # Add the cache directory argument if provided
    if cache_dir:
        sys.argv.extend(["-c", cache_dir])

    # Run the 'onnxruntime_genai.models.builder' module directly within the script
    try:
        runpy.run_module('onnxruntime_genai.models.builder', run_name="__main__")
        reset_module_cache('onnxruntime_genai.models.builder')  # Reload the module after execution to release locks
    except Exception as e:
        print(f"Error occurred while running model builder: {e}")
        sys.exit(1)

def handle_tokenizer_and_files(input_dir, output_dir):
    """
    Function to handle deleting and copying tokenizer.json and other necessary files.
    """
    tokenizer_file = "tokenizer.json"
    
    # Create the output directory if it doesn't exist
    if not os.path.exists(output_dir):
        print(f"Creating output directory: {output_dir}")
        os.makedirs(output_dir)
    
    # Delete the tokenizer.json file from the output directory if it exists
    output_tokenizer_path = os.path.join(output_dir, tokenizer_file)
    if os.path.exists(output_tokenizer_path):
        print(f"Deleting {output_tokenizer_path}")
        os.remove(output_tokenizer_path)

    # Copy the tokenizer.json file from the input directory to the output directory
    input_tokenizer_path = os.path.join(input_dir, tokenizer_file)
    if os.path.exists(input_tokenizer_path):
        print(f"Copying {input_tokenizer_path} to {output_dir}")
        shutil.copy(input_tokenizer_path, output_dir)
    
    # Copy all relevant files from input to output directory
    for file_name in os.listdir(input_dir):
        input_file_path = os.path.join(input_dir, file_name)
        output_file_path = os.path.join(output_dir, file_name)

        # Skip directories, .gitattributes, .safetensors, and model.safetensors.index.json files
        if os.path.isdir(input_file_path) or \
           file_name == ".gitattributes" or \
           file_name.endswith(".safetensors") or \
           file_name == "model.safetensors.index.json":
            continue

        # Copy the file if it doesn't already exist in the output directory
        if not os.path.exists(output_file_path):
            print(f"Copying {input_file_path} to {output_file_path}")
            shutil.copy(input_file_path, output_file_path)

def main():
    # Set up argument parsing
    parser = argparse.ArgumentParser(description='ONNX Model Conversion for Embedded Python Environment')
    
    # Add positional arguments for the input and output paths, precision, and execution provider
    parser.add_argument('-i', '--input', type=str, required=True, help='Path to input model')
    parser.add_argument('-o', '--output', type=str, required=True, help='Path to output ONNX model')
    parser.add_argument('-p', '--precision', type=str, choices=['int4', 'fp16', 'fp32'], default='int4', help='Precision for model conversion')
    parser.add_argument('-e', '--execution', type=str, choices=['cpu', 'cuda', 'dml'], required=True, help='Execution provider')
    
    # Add an optional argument for the cache directory
    parser.add_argument('-c', '--cache_dir', type=str, help='Cache directory for temporary files')

    # Parse the provided arguments
    args = parser.parse_args()

    # Run the model builder using the provided arguments
    run_model_builder(args.input, args.output, args.precision, args.execution, args.cache_dir)

    # Handle tokenizer and file management after the conversion
    handle_tokenizer_and_files(args.input, args.output)
    

if __name__ == "__main__":
    main()
