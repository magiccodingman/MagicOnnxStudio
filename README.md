# Magic Onnx Studio
Easy GUI interface to convert and test AI models using DirectML, Cuda, and CPU.

If you want to convert an AI model to DirectML, Cuda, or CPU, you normally have to have 3 separate python instances. Each python instance needs a different setup. Then to test the AI models, you have to test each with ONNX libraries that also must be in their own separate instances. Lets make this process easier! Once place, one application, one instance!

## Minimum Hardware Requirements
OS: Windows 10/11 x64
RAM: 16 GB
Available Space: 150 GB

## Recommended Hardware Requirements
OS: Windows 10/11 x64
RAM: 128 GB
Drive Space: 200+ GB

#### Hardware Requirements Notes
Though the minimum will likely work, there's things to understand about this process that I cannot change. Unless you have 128 GB of RAM or more, the process will use your RAM and disk during the conversion process. I have 64 GB of RAM because 128 GB of RAM with DDR5 as of today is wonky. But even small AI models can spike useage to 120+ GB of RAM required. And once your RAM is filled up, it'll utilize your disk space. More RAM will be faster, but it's fine if disk is used, it's just slower. But that's why the drive storage requirements are so large. If you have 128 GB to 256 GB of RAM, then you'll likely only need something like 10-15 GB of additional drive space. But I'm sure it also really depends on the models you're converting as well. I've mostly been converting smaller models, so just be aware that during the conversion process, you'll need lots of memory/disk!


#### Limitations
Currently this only works with Windows due to how the code works with embedded environments. This can change without too much issues in the future. I bricked my Linux workstation accidentally and didn't have snapshots like a real chad. And due to time constraints, I didn't have the heart to rebuild my OS exactly as i needed it, so I'm back on Windows for the time being. Therefore, it's not exactly easy for me to unit test for Linux at the moment. So, until further notice or until someone converts some of the code to work accordingly for linux and submits a merge request. This application will only work on Windows as of right now. I'm sorry Linux users!

## How Do I convert AI models to ONNX in one application?
Easy! I handled all the hard parts for you obviously! The process is identical to how it'd normally work except I just did it for you. What's normally needed to convert AI models to ONNX? You must understand this process to understand the simplicity of my resolution.

#### Normal process to convert AI models to ONNX
We first must setup a Python Anaconda environment for the CPU, DirectML, Cuda 11, and Cuda 12 runtimes. I wish we could just use a single instance, but the way the libraries were coded, they cannot overlap. Each instance must be unique to the process required. After you setup each instance you must do the following:

**CPU Environment Pip Commands**
```py
pip install torch transformers onnx onnxruntime
pip install --pre onnxruntime-genai
```

**DirectML Environment Pip Commands**
```py
pip install torch transformers onnx onnxruntime-directml
pip install --pre onnxruntime-genai-directml
```

**Cuda 11 Environment Pip Commands**
```py
pip install torch==1.13.1+cu117 torchvision==0.14.1+cu117 torchaudio==0.13.1 --index-url https://download.pytorch.org/whl/cu117
pip install --pre onnxruntime-genai-cuda --index-url https://aiinfra.pkgs.visualstudio.com/PublicPackages/_packaging/onnxruntime-cuda-11/pypi/simple/
```

**Cuda 12 Environment Pip Commands**
```py
pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu122
pip install --pre onnxruntime-genai-cuda
```

## How My Code Works
My process is nearly identical to the normal process, I just automated the process a bit. When the application launches, it'll add to an output directory a Python 3.10.6 Windows 64x embedded directory with the base files. I then copy this 4 times to create a seperate embedded environment for CPU, DirectML, Cuda 11, and Cuda 12. I then run the, "get-pip.py" file that I placed in each of the embedded environments to setup pip. After that, I created some scripts to help me utilize the embedded environments. like the "pip_runner.py" and the "onnx_runner.py". I then made some simple C# code to utilize these scripts and run the pip install commands for each environment on your behalf. As of right now, the end size of these files is under 8GB for me. Now that each are properly setup. When you want to convert an AI model to any of the 4 conversion types, I use my "onnx_runner.py" to utilize the target embedded environment and help run the commands on your behalf!

Honestly it's not too complicated. There's no special sauce here. Nothing wild is going on. I simply automated this process. Why did I do this though? Because I'm very lazy. I hate doing this manually. I hate writing documentation so that I remember the process when I need to do it again a few months down the line. I just want an easy GUI to do it all for me. Don't you?
