# Google Calendar Skill

This skill allows the authenticated user to perform simple tasks on his/her Google Calendar account directly from the VertexGraph chat prompt.

## API Permissions Setup

 - Step 1: Go to the Google Cloud Console at https://console.cloud.google.com/apis/dashboard
 - Step 2: Sign in with the account that you want to expose to the skill
 - Step 3: On the left-side navigation, click on the "Enabled APIs & Services"
![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/Enabled_APIs_and_Services.png)

 - Step 4: Enable the Calendar API
![Enable the Calendar API Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/Enable_the_Calendar_API.png)
 - Step 5: On the left side navigation. Click on Credentials. Create a new OAuth 2.0 Client ID
![Credentials Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/Credentials.png)

 - Step 6: On the OAuth 2.0 Client ID details page, under Authorized JavaScript origins, add: https://api.vertexgraph.ai. Under the Authorized redirect URIs, add: https://api.vertexgraph.com/adminapi/assets/oauthcode
![Credentials URLs Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/Credentials_URLs.png)

## IIS Installation

The skill requires a Windows Server with Internet Information Services (IIS) with dotnetcore 3.1 (install dotnet-hosting-3.1.32-win.exe or higher on your windows server. The recommended setup is to create a web application folder under the Default Web Site node which points to the compiled files for this project. Create a separate application pool with the permissions to access the skill's output directory.

![Add Application Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/IIS_Add_Application.png)

After downloading the source files to a folder on your computer, open the solution (.sln) file with Visual Studio 2022 or higher and compile it. DotNet will automatically create a subfolder under the root/bin folder called:
[projectfolder]\bin\Release\netcoreapp3.1
Use this full path as the destination path for your IIS web application folder.

![Application Basic Settings Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GCalendar/images/IIS_Application_Basic_Settings.png)

## Testing the Setup

If the web application is set up correctly, you should be able to access the following URL from your browser:
http://localhost/webappnamehere
*Replace webappnamehere with the actual name of your web application. Example: "gcalendar"*

If the setup is correct you should see the contents of the /Templates/ai-plugin.json, which is an embedded resource in the Visual Studio project and is routed using the metatags on the Controllers/SkillController.cs file.

To ensure that the routing is working as intended, try to access the following URL from your browser:
http://localhost/webappnamehere/skills/messages/test
If the above is set up correctly, you should see a "hello world" phrase on the page.

Once you get the above steps working, you can point an external domain's A Record to your public IPv4 address. Alternatively, you can use a proxy, such as CloudFlare, which makes the process of generating external URLs easier.

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
 - Step 9: Search for the skill name, which should be Gcalendar in this case.

## Test Prompts

Now that the Google Calendar Skill is properly installed, you can try the following example prompts:

 - "Give me all events happening within today"
 - "Create an event called 'testing event' starting at 6pm and ending at 8pm today and add example@example.com as an attendee
 - "Remove all events from my calendar within next week"
 - "Get instances of recurring events within the last 2 days"
 - "Remove my meeting event with example@example.com"
 - "Remove John Doe from the list of attendees of the event 'event example'"
 - Create a new calendar event
