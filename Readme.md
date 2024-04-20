# Provision and deploy a .NET Aspire app


## Using the Azure Developer CLI (azd)

The steps in this section demonstrate how to handle 
provisioning and deploying of a .NET Aspire app to Azure using `azd`:
  
- Create a new .NET Aspire application using Visual Studio Preview.  
- Important: If you are on Windows, and your user has an extra whitespace character, use the following command
through a PowerShell terminal before continuing. You can find more details in [this GitHub issue](https://github.com/Azure-Samples/azure-search-openai-demo/issues/502):

```
$env:AZD_CONFIG_DIR="C:\azdConfig"
```

- Update the vulnerable packages in all projects of the template.
- Execute the `azd init` command to initialize your project with `azd`, 
which will inspect the local directory structure and determine the type of app:

```
azd init
```

- Select **Use code in the current directory** when `azd` prompts you.
- After scanning the directory, `azd` prompts you to confirm that it found the correct 
.NET Aspire *AppHost* project. Select the **Confirm and continue initializing my app** option.

- Finally, specify the the environment name, which is used to name provisioned resources
in Azure and managing different environments such as `net-aspire-dev`:
  - `azd` generates a number of files and places them into the working directory. These files are:
	- *azure.yaml*: Describes the services of the app, such as .NET Aspire AppHost project, and maps them to Azure resources.
	- *.azure/config.json*: Configuration file that informs `azd` what the current active environment is.
	- *.azure/net-aspire-dev/.env*: Contains environment specific overrides.
	- *.azure/net-aspire-dev/config.json*: Configuration file that informs `azd` which services should have a public endpoint in this environment.
- The *azure.yaml* file has a `project` field pointing to a .NET Aspire AppHost project. 
With this, `azd` activates its integration with .NET Aspire 
and derives the required infrastructure needed to host this 
application from the application model specified in the *Program.cs* file 
of the .NET Aspire app.
- The *.azure\net-aspire-dev\config.json* file is how `azd` remembers 
(on a per environment basis) which services should be exposed with a public endpoint. 
`azd` can be configured to support multiple environments.

### Initial deployment
In order to deploy the .NET Aspire application, authenticate to Azure AD 
to call the Azure resource management APIs.

```
azd auth login
```

The previous command will launch a browser to authenticate the command-line session.  

Once authenticated, use the following command to provision and deploy the application.

```
azd up
```

When prompted, select the subscription and location the resources should be deployed to. 
Once these options are selected the .NET Aspire application will be deployed.

### Deploy application updates
To speed up deployment of code changes, `azd` supports deploying code updates
in the container image. This is done using the `azd deploy` command.  

### Deploy infrastructure updates
Whenever the dependency structure within a .NET Aspire app changes, 
`azd` must re-provision the underlying Azure resources. 
The `azd provision` command is used to apply these changes to the infrastructure.  

Remember to clean up the Azure resources that you've created during this walkthrough. 
Because `azd` knows the resource group in which it created the resources it can
be used to spin down the environment using the following command:

```
azd down
```

### Generate Bicep from .NET Aspire app model
Although development teams are free to use `azd up` (or `azd provision` and `azd deploy`) 
commands for their deployments both for development and production purposes, 
some teams may choose to generate Bicep files that they can review and manage 
as part of version control (this also allows these Bicep files to be 
referenced as part of a larger more complex Azure deployment).  
`azd` includes the ability to output the Bicep it uses for provisioning via following command: 

```
azd config set alpha.infraSynth on
azd infra synth
```

After this command is executed, the following files are created:  
- *infra/main.bicep*: Represents the main entry point for the deployment.
- *infra/main.parameters.json*: Used as the parameters for main Bicep (maps to environment variables defined in *.azure* folder).
- *infra/resoures.bicep*: Defines the Azure resources required to support the .NET Aspire app model.
- *GlobalAzure.NetAspire.Server/manifests/containerApp.tmpl.yaml*: The container app definition for `aspiredemoapp` project resource.
- *GlobalAzure.NetAspire.Api/manifests/containerApp.tmpl.yaml*: The container app definition for `aspiredemoapi` project resource.
  
The *infra\resources.bicep* file doesn't contain any definition of 
the container apps themselves (with the exception of container apps 
which are dependencies such as Redis and Postgres)  

The definition of the container apps from the .NET service
projects is contained within the *containerApp/tmpl.yaml* files in the `manifests`
directory in each project respectively.
  
After executing the `azd infra synth` command, when `azd provision` and `azd deploy`
are called they use the Bicep and supporting generated files.  
  
Because `azd` makes it easy to provision new environments, 
it's possible for each team member to have an isolated cloud-hosted environment 
for debugging code in a setting that closely matches production. 
When doing this each team member should create their own environment 
using the following command:

```
azd env new
```
  
This will prompt the user for subscription and resource group information 
again and subsequent `azd up`, `azd provision`, and `azd deploy` invocations 
will use this new environment by default. 
The `--environment` switch can be applied to these commands to switch between environments.  

## Using a GitHub Actions workflow file
  
Although `azd` generated some essential template files for you, the project still needs 
a GitHub Actions workflow file to support provisioning and deployments using CI/CD
in an automated way:  
- Create an empty *.github* folder at the root of your project. 
`azd` uses this directory by default to discover GitHub Actions workflow files.
- Inside the new *.github* folder, create another folder called *workflows* 
(you'll end up with *.github/workflows*).
- Add a new GitHub Actions workflow file into the new folder named *azure-dev.yml*,
or use the current created one.

### Create the GitHub repository and pipeline
  
The Azure Developer CLI enables you to automatically create CI/CD pipelines 
with the correct configurations and permissions to provision and deploy resources to Azure.
`azd` can also create a GitHub repository for your app if it doesn't exist already.  
- Run the `azd pipeline config` command to configure your 
deployment pipeline and securely connect it to Azure:

```
azd pipeline config
```

- Select the subscription to provision and deploy the app resources to.
- Select the Azure location to use for the resources.
- Enter y to proceed when `azd` prompts you to commit 
and push your local changes to start the configured pipeline.