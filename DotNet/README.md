# AI Skills Hosting Setup

## IIS Installation

Custom skills require a Windows Server and Internet Information Services (IIS) with dotnetcore 3.1 Hosting Bundle (dotnet-hosting-3.1.32-win.exe or higher from https://dotnet.microsoft.com/en-us/download/dotnet/3.1). The recommended setup is to create a separate web application folder under the Default Web Site node for each custom skill. Each web application folder should have its own application pool so it runs in a separate memory space, and should point to the publishing directory of each Visual Studio project.

![Add Application Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/IIS_Add_Application.png)

After downloading the source files to a folder on your computer, open the solution (.sln) file with Visual Studio 2022 or higher and compile it. Visual Studio will automatically create a subfolder under the "bin" folder called netcoreapp3.1:
[projectfolder]\bin\Release\netcoreapp3.1
Use the full path similar the one above as the destination path for your IIS web application folder (under Basic Settings...).

![Application Basic Settings Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/IIS_Application_Basic_Settings.png)

Please make sure the IIS application pools and credentials under the web application's basic setting window have the proper NTFS permissions to access the publishing directory. To grant NTFS permissions on a folder to a specific IIS application pool, you can use the following syntax in the NTFS permissions popups to resolve an application pool name: "IIS APPPOOL\\[ApplicationPoolNameHere]" (remove the quotes and replace the [ApplicationPoolNameHere] with the actual name).

![NTFS Permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/NTFS_Permissions.png)

## Testing the Skill Server Setup

If the web application is set up correctly, you should be able to access the following URL from any browser on the server itself:
http://localhost/[WebAppNameHere] (*Replace [WebAppNameHere] with the actual name of your web application. Example: "skill"*). The contents of the /Templates/ai-plugin.json, which is an embedded resource in the Visual Studio project is routed from the Controller code of the application, and should be rendered on the page.

To ensure that the routing is working as intended, try to access the following URL from your browser:
http://localhost/[WebAppNameHere]/apidefs
If the above is set up correctly, you should see the available API definitions in YAML format. This page describes the endpoints and parameters that can be accessed via REST (REpresentational State Transfer) requests.

Once you get the above steps working, you can download the Gateway Agent from the VertexGraph.ai website and install it on the same server. The Gateway Agent will allow the skills to be accessible via localhost from the vertexgraph.ai domain. To download the Gateway Agent, set up a New On-Premises Site.

![New On-Premises Site Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/New_On_Premises_Site.png)

![Download Gateway Agent Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/Download_Gateway_Agent.png)

## Setting up a Custom Skill in VertexGraph

1. Navigate to [https://vertexgraph.ai/portal/](https://vertexgraph.ai/portal/)
2. On the left side tool bar select the second option “AI”
3. Now select the custom skills icon on the top right corner

![AI Assistants Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/images/AI_Assistants.png)

1. Once on the skills section Press “New Custom Skill”
2. Follow the setup until you reach the section where you must enter the OAuth Client ID and Secret
3. Copy and paste the client ID and secret from the Custom App (read the README.md file for each skill for details on how to set up the custom app with the necessary permissions and OAuth Secret keys)
4. Get the Authorization Code and authorize VertexGraph to access your app's account
5. Once completed assign the new skill to any of your assistants to start using it
6. After the setup, open the AI assistant and ask some test prompts. The README.md file for each skill has some test prompts that you can try.
