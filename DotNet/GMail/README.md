﻿# Gmail Skill

This skill allows the authenticated user to perform simple tasks on his/her gmail account directly from the vertexgraph chat prompt.

## IIS Installation

The skill requires an IIS server with dotnetcore 3.1 (install dotnet-hosting-3.1.32-win.exe or higher on your windows server. The recommended setup is to then create a web application folder under the Default Web Site node which points to the compiled files for this project.

After downloading the source files to a folder on your computer, open the solution sln file with Visual Studio 2022 or higher and compile the project. DotNet will automatically create a subfolder under the root/bin folder called:
[projectfolder]\bin\Release\netcoreapp3.1
Use this full path as the destination path for your IIS web application folder.

## Testing the Setup

If the web application is set up correctly, you should be able to access the following URL from your browser:
http://localhost/webappnamehere
*Replace webappnamehere with the actual name of your web application. Example: "gmail"*

If the setup is correct you should see the contents of the /Templates/ai-plugin.json, which is an embedded resource in the Visual Studio project and is routed using the metatags on the Controllers/SkillController.cs file.

To ensure that the routing is working as intended, try to access the following URL from your browser:
http://localhost/webappnamehere/skills/messages/test
If the above is set up correctly, you should see a hello world phrase on the page.

Once you get the above working, you can point an external domain's A Record to your public IPv4 address from your domain name's DNS records. Alternatively, you can use a proxy, such as CloudFlare, which makes the process of generating external URLs easier.

## VertexGraph Installation

Now you can install the custom skill on the vertexgraph.ai interface.

 - Step 1: From the AI Assistants page, click on the Custom Skills icon on the top-right of the page.
 - Step 2: Click on the New Skill Button on the top-left of the page.
 - Step 3: Choose the Open API Plugin tile.
 - Step 4: Optional: Enter a description and click on the Continue button
 - Step 5: Enter the full external URL of the skill and click on the Continue button. For example: https://myexternalurlhere.example.com/webappnamehere
 - Step 6: Authorize the skill on the OAuth authentication screen and finish the installation.
 - Step 7: Return to the AI Assistants page and click on the manage button on the Default Assistant tile.
 - Step 8: From the Skills tab, click on the Assign Skill button on the top-right of the page.
 - Step 9: Search for the skill name, which should be Gmail in this case.

## Testing Prompts

Now that the Gmail Skill is properly installed, you can try the following example prompts:

 - "How many emails have I received from support in the last 2 days?"
 - "Show me the unsummarized body of the last email from myfriend@example.com"
 - "Send an email to myself@example.com with subject 'hello world' and body 'this is a test email from the gmail skill'"
 - "Add a label called 'hello there' to the email containing the subject 'hello world'"
 - "Search for emails containing the label 'hello there'"
 - "Reply to the email from myfriend@example.com from today. Tell him that I approve the message"
 - "Show me a summary of the last 5 emails from support@example.com"
 - ...and many more
