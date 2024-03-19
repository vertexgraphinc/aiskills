# AI Skills Local Setup (optional)

## IIS Installation

The skill requires an IIS server with dotnetcore 3.1 (install dotnet-hosting-3.1.32-win.exe or higher on your windows server). The recommended setup is to then create a web application folder under the Default Web Site node which points to the compiled files for this project.

After downloading the source files to a folder on your computer, open the solution (.sln) file with Visual Studio 2022 or higher and compile it. DotNet will automatically create a subfolder under the root/bin folder called:
[projectfolder]\bin\Release\netcoreapp3.1
Use this full path as the destination path for your IIS web application folder.

## Testing the Setup

If the web application is set up correctly, you should be able to access the following URL from your browser:
http://localhost/webappnamehere (*Replace webappnamehere with the actual name of your web application. Example: "skill"*)

If the setup is correct you should see the contents of the /Templates/ai-plugin.json, which is an embedded resource in the Visual Studio project and is routed using the metatags on the Controllers/SkillController.cs file.

To ensure that the routing is working as intended, try to access the following URL from your browser:
http://localhost/webappnamehere or http://localhost/webappnamehere/apidefs
If the above is set up correctly, you should see the basic application info and available api definitions

Once you get the above steps working, you can point an external domain's A Record to your public IPv4 address. Alternatively, you can use a proxy, such as CloudFlare, which makes the process of generating external URLs easier.

## VertexGraph Installation

Now you can install the custom skill on the vertexgraph.ai interface.

 - Step 1: From the AI Assistants page, click on the Custom Skills icon on the top-right of the page.
 - Step 2: Click on the New Skill Button on the top-left of the page.
 - Step 3: Choose the Open API Plugin tile.
 - Step 4: (Optional) Enter a description and click on the Continue button
 - Step 5: Enter the full external URL of the skill and click on the Continue button. For example: https://myexternalurlhere.example.com/webappnamehere
 - Step 6: Authorize the skill on the OAuth authentication screen and finish the installation.
 - Step 7: Return to the AI Assistants page and click on the manage button on the Default Assistant tile.
 - Step 8: From the Skills tab, click on the Assign Skill button on the top-right of the page.
 - Step 9: Search for the skill name, which should be the name of the folder containing the respective project.