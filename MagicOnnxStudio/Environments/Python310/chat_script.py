import os
import sys

# Get the current script directory
script_dir = os.path.dirname(os.path.abspath(__file__))

# Construct paths for key directories
lib_dir = os.path.join(script_dir, 'Lib')
site_packages_dir = os.path.join(lib_dir, 'site-packages')

# Add the necessary paths to sys.path
sys.path.insert(0, lib_dir)
sys.path.insert(0, site_packages_dir)

import argparse
import onnxruntime_genai as og

# Global model and tokenizer variables to persist them
model = None
tokenizer = None

def load_model(model_path):
    global model, tokenizer
    try:
        model = og.Model(model_path)
        tokenizer = og.Tokenizer(model)
        print(f"Model loaded from {model_path}.")
        print("AI model is ready, but note that the base chat template being used is generic.")
        print("The AI model may not respond with answers that fully demonstrate its capabilities.")
        print("This environment is intended for easy testing with no saved history, so the AI only remembers the current question.")
    except Exception as e:
        print(f"Error loading model: {e}")
        sys.exit(1)

def chat(input_text):
    global tokenizer, model
    if model is None or tokenizer is None:
        print("Error: No model loaded. Please load a model first using --model.")
        return
    
    # Create the chat template with the user's input
    chat_template = '<|user|>\n{input} <|end|>\n<|assistant|>'
    if not input_text:
        print("Error, input cannot be empty.")
        return

    prompt = chat_template.format(input=input_text)
    

    # Tokenize the prompt
    input_tokens = tokenizer.encode(prompt)

    params = og.GeneratorParams(model)
    
    # Set sensible default max_length for the model
    search_options = {'max_length': 2048}
    params.set_search_options(**search_options)
    params.input_ids = input_tokens

    generator = og.Generator(model, params)
    tokenizer_stream = tokenizer.create_stream()

    print("Output: ", end='', flush=True)
    try:
        while not generator.is_done():
            generator.compute_logits()
            generator.generate_next_token()
            new_token = generator.get_next_tokens()[0]
            print(tokenizer_stream.decode(new_token), end='', flush=True)
    except KeyboardInterrupt:
        print("  --control+c pressed, aborting generation--")
    print()


def dispose_model():
    global model, tokenizer
    if model is not None:
        del model
    if tokenizer is not None:
        del tokenizer
    print("Model disposed. Exiting...")
    sys.exit(0)

# Command-line parser for initial model loading
parser = argparse.ArgumentParser(description="CLI for interacting with ONNX Runtime GenAI models")

parser.add_argument('--model', type=str, help='Path to the AI model file')
args = parser.parse_args()

# Load the model at the start if provided
if args.model:
    load_model(args.model)

# Enter a simple input loop for chatting with the model
try:
    while True:
        text = input("\nInput: ")
        chat(text)
except KeyboardInterrupt:
    dispose_model()
