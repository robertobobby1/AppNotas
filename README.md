# AppNotas

## Description

This is a simple phone application compatible with IOS and Android. It is a notes app where you can create several sections/folders recursively and in the end files can be created. It contains a simple text editor that allows several simple actions and other more complex such as uploading files. It has been implemented using a sqlite local database that persists all the necessary information in the local storage of the phone.

## Installation

In all cases you will need to download Visual Studio 2022/Visual Studio 2022 for Mac. This is what contains the simulators and necessary sdk's to build the application for either target(ios/android). Specifically in Mac Visual Studio will force you to install Xcode to get the necessary simulators and command line tools needed. After this just run:

```
git clone https://github.com/robertobobby1/AppNotas.git
```

Open the .sln in the root of the project with Visual studio and build the solution from there.

## Running simulation

To run a simulation in your computer you will need to select at the top left of the Visual Studio IDE the target(IOS/Android) the Configuration(Debug/Release) and this will run the simulator, build the application and deploy it to the device. Here you can already navigate through the app and investigate its features.

## Building APK

To build the apk to use it in a mobile phone you will need to follow the instructions given by microsoft: https://learn.microsoft.com/en-us/previous-versions/xamarin/android/deploy-test/release-prep/
