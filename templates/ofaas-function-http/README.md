# OpenFaaS HTTP Trigerd Function Template
dotnet 5.0 cli templates for OpenFaas

# How to install the template
To install the template clone the reprository to your local environment and run

1. For the general template that creates a function with http trigger | cron | event source, run
~~~
dotnet new -i ./ofaas-function
~~~

2. For a function with only an HTTP trigger run 
~~~
dotnet new -i ./ofaas-function-http 
~~~

# Create a Function Project
To create a function project run 

~~~
dotnet new ofaas-function --func-name <name of your function>
~~~

You will have the same developer experience as any other dotnet web project. Since this is a web project you can add packages, build and debug localy using VS/VS Code.

You will use the faas cli to build, push and deploy your functon to download it open a new terminal and enter the follwoing line

## Linux or Mac
~~~
$ curl -sSL https://cli.openfaas.com | sh
~~~

## Windows
Open a PowerShell terminal and run
~~~
$version = (Invoke-WebRequest "https://api.github.com/repos/openfaas/faas-cli/releases/latest" | ConvertFrom-Json)[0].tag_name
(New-Object System.Net.WebClient).DownloadFile("https://github.com/openfaas/faas-cli/releases/download/$version/faas-cli.exe", "faas-cli.exe")
~~~

## Build 
In order to build your function your first need to get the dockerfile template by running the following command 
~~~
$ faas-cli template store pull dockerfile
~~~ 
This will download the oficial openfaas templates and put them into the template directory. make sure to pull the template in the same location where you .yml file located.

To build your function run
~~~
$ faas-cli build -f <your-function>.yml
~~~

## Push
To push your function image run 
~~~
$ faas-cli push -f <your-function>.yml
~~~

## Deploy
First make sure you are logged in to you OpenFaaS gateway if you need to start a session run
~~~
$ faas-cli login -p <your password>
~~~
To deploy run
~~~
$ faas-cli deploy -f <your-function>.yml
~~~

# Final Notes

Please note that the templates are still in alpha if you find any issues please open and issue or if you like to contribute create a Pull Request.
