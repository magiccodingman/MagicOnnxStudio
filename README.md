# Magic Onnx Studio
Easy GUI interface to convert and test AI models using DirectML, Cuda, and CPU.

If you want to convert an AI model to DirectML, Cuda, or CPU, you normally have to have 3 separate python instances. Each python instance needs a different setup. Then to test the AI models, you have to test each with ONNX libraries that also must be in their own separate instances. Lets make this process easier! Once place, one application, one instance!

#### Limitations
Currently this only works with Windows due to how the code works with embedded environments. This can change without too much issues in the future. I bricked my Linux workstation accidentally and didn't have snapshots like a real chad. And due to time constraints, I didn't have the heart to rebuild my OS exactly as i needed it, so I'm back on Windows for the time being. Therefore, it's not exactly easy for me to unit test for Linux at the moment. So, until further notice or until someone converts some of the code to work accordingly for linux and submits a merge request. This application will only work on Windows as of right now. I'm sorry Linux users!

## How Do I convert AI models to ONNX in one application?
Easy! I handled all the hard parts for you obviously! The process is identical to how it'd normally work except I just did it for you. What's normally needed to convert AI models to ONNX? You must understand this process to understand the simplicity of my resolution.
