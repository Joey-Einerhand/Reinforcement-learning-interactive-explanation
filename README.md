# Reinforcement-learning-interactive-explanation
A small interactive experience which aims to teach how reinforcement learning works.

## 1. Usage

This chapter contains instructions for running a completed version of the application (builded version).

### 1.1. Running with Docker

The easiest way to run the application is trough a Docker container. For this you don't have to install any of the dependencies. You only need to have [Docker](https://docs.docker.com/get-docker/) installed on your machine. In order to run the application inside a Docker container you can follow the following steps:

1. Download the Docker image from the Github repository. This file is called 'demonstrator_image.tar', and you can find it inside the 'Docker_image'-folder.
2. Navigate to the folder with the downloaded image using the terminal.
3. Run the following command `docker load -i demonstrator_image.tar` to load the image into Docker.
4. Run the following command `docker run -p 8080:80 -d demonstrator` to run an instance of the image inside a Docker container.
5. Navigate to the following URL [http://localhost:8080/test/](http://localhost:8080/test/) to interact with the container.

### 1.2. Running with a webserver

It is also possible to run the application yourself on a webserver. For this you can use the html-file that is used in the Docker environment. You can find the latest html-file called 'index.html' inside the 'Docker_build'-folder. We recommend running the html file on a webserver*. This guide won't go into detail on how to set up a web server, but we can recommend [NGINX](https://nginx.org/en/)**. NGINX is free (as in beer and speech. It has no cost and is open source with permissive license).
If you have a modern IDE and have a recent version php installed as an interpreter, it might be able to run the web server which is included in recent versions of php. 

If done correctly, clicking the 'play' button on the HTML page loads and runs the application correctly.

*You _might_ be able to run this file in your browser and make it work, but this won't work for most (modern) browsers. The 'website' you're running needs access to the files on the device you're running it, and most browsers won't allow "plainly" run html pages to access storage on your device, for security reasons.

** Alternatively, you can upload your files to a website that can host unity games. We recommend [www.itch.io](https://joeyehand.itch.io/reinforcement-learning). It's free. You could theoretically also run it in GitHub Pages (free), but this hasn't been tested.


## 2. Editing

This chapter contains instructions for editing the source code of the application.  

### 2.1. Prerequisites

To be able to edit, run, and build the source code, you need to install the engine this application is built on. To install the engine, install:

* Unity (Application)
  1. Install the [**Unity hub**](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html). Any recent version is fine.
  2. In unity hub, navigate to 'installs' install the **unity installation** which the application uses (if you open the solution as described in chapter 2.2. 'installation', it will display which version you need). The current used unity installation version is `unity version 2020.3.11f1  `
  3. Be sure to [**install the 'webgl' module**](https://docs.unity3d.com/Manual/GettingStartedAddingEditorComponents.html) if you want to build the game so it runs on the web.

* Python (Machine learning) 
  1. Install a [version of python](https://www.python.org/downloads/). We recommend getting python 3.9.0 or higher. 

### 2.2. Installation

1. Download the source code on your local machine. If you don't know how to do this, [GitHub has a nice how-to guide.](https://docs.github.com/en/desktop/contributing-and-collaborating-using-github-desktop/adding-and-cloning-repositories/cloning-a-repository-from-github-to-github-desktop)
2. [Add the source code as a project to your Unity Hub](https://docs.unity3d.com/2019.1/Documentation/Manual/GettingStartedOpeningProjects.html)
3. Install the necessary Python libraries:
   1. Create a python virtual environment. Open a CMD window, navigate to the local copy of this repo on your PC, and run the command `py -m venv venv`. This creates a python virtual environment (and folder) called 'venv' in the repo.
   2. run the command `venv\Scripts\activate` to activate the virtual environment.
   3. Run the command `py -m pip install -r requirements.txt` to install  all necesarry packages into that virtual environment. 

### 2.3. How to use machine learning in this project?

After completing the 2.2. Installation, I recommend following along with [this](https://www.youtube.com/watch?v=zPFU30tbyKs) tutorial if you want to learn how to use machine learning in unity. All knowledge in this video transfers to being able to run machine learning in this project.

## 3. Building

This chapter contains instructions for building the application.  

## 3.1. Building the Unity project

This chapter will discuss how to build the application to WebGL, to make it able to run in the web browser. For building for other platforms, see the [Unity Docs](https://docs.unity3d.com/Manual/BuildSettings.html).

1. Make sure you've installed the WebGL module as per instructions in chapter 2.1.
2. Open the application's unity project in unity hub
3. In the top left, open the `file` menu and click on `build settings ...`
4. On the bottom left of the build settings prompt, you can select which platform to target when building. Click on the `WebGL` platform option, and click on the `Switch Platform` button on the bottom right. If this option was greyed out and the platform was already selected, proceed to step 5. If Unity prompts you to install a WebGL module, follow along with it's instructions.
5. Click on the `build` button on the bottom right. Select a folder to save the build in (you probably don't want to build inside your local copy of the repo, so make or navigate to a folder outside of your local repo)
6. Click `select folder` and wait for Unity to build the application. After Unity's done, navigate to the folder you built the solution in. If everything's correct, there should be _at least_ one .html-file and a folder should be in the folder you selected.

Now you have a builded version of the application in the form of a html-file. Now continue with 3.2. to build a Docker container in order to run the builded application on a webpage or go back to 1.2. to run the html-file on a webserver yourself.

### 3.2. Building a Docker container

To run the the application using Docker you need to have a builded version of the applications using [WebGL](https://get.webgl.org/) in the form of a html-file. In order to get this version you can head over to chapter 3.1. building the Unity project. You also need to have [Docker](https://docs.docker.com/get-docker/) installed on your machine. When you meet these requirements you can use the folowing instructions to run your build inside a Docker-container.

1. Download the DockerFile for the application from GitHub. This file is located inside the 'Docker_build'-folder.
2. Place the Dockerfile inside the folder of the build, inside the same directory as the index.html-file.
3. Navigate to the folder of the build using the terminal.
4. Run the following command `docker build -t demonstrator .` to build an image of the build using the DockerFile.
5. Run the following command `docker run -p 8080:80 -d demonstrator` to run an instance of the image inside a Docker container.
6. Navigate to the following URL [http://localhost:8080/test/](http://localhost:8080/test/) to interact with the container.

### 3.3 Building a Docker image

In order to easily share the Docker image of the builded version of the application you can export the Docker image to a .tar-file. For instructions on how to export a Docker image to a .tar-file you can use the following steps:

1. Navigate to the desired folder where te Docker image will be exported to using the terminal.
2. Run the following command `docker save demonstrator -o demonstrator_container.tar` to export the Docker image to a .tar-file. Pay attention to the naming of the image and the .tar-file.

You can go back to 1.1. Running with Docker to follow the instructions in order to convert the .tar-file back into a Docker image and run the application on a webpage inside a Docker container. 

## 4. Contributing

This chapter contains information for contributing to the Github repository. 

To contribute to this repository, please create a branch or fork and make your edits. When you've finished making your changes, go back to the [pull request section of the original repo](https://github.com/Joey-Einerhand/Reinforcement-learning-interactive-explanation/pulls) and make a pull request. If the GitHub bot indicates a merge conflict, please fix it. If there are no conflicts, a code review is required by one of the primary contributors. The pull request can be merged if the code reviewer gives permission.
To become a code reviewer, please send me, [Joey](https://github.com/Joey-Einerhand), a message. 

## 5. Can I use this in <x>?

This chapter contains information for reuse of the project.
  
Yes. This project is dedicated to the public domain, which means you're allowed to do whatever you want with it. The aim is to allow anyone to be able to use this project (for example, teachers) to do anything with the project without any fear of legal repercussions.

