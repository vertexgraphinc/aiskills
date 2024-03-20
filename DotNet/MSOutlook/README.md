# Microsoft Outlook Skill

This skill allows the authenticated user to perform simple tasks on his/her Microsoft Outlook account directly from the VertexGraph chat prompt.

## API Permissions Setup

1. Go to <https://entra.microsoft.com>
2. Login with your Microsoft account credentials.
3. Go to **Applications > App registrations.**

![App Registrations Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/App_Registrations.png)

4. Select **New registrations**. Fill out the desired name for the registration and then choose second or third option in **Supported account types.** You can leave Redirect URI empty.

![Register an Application Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Register_an_Application.png)

5. Select **Register** to create a new app registration.
6. After that is done, navigate back to App registrations in step 3 and click on your newly create application.
7. Now navigate to **API Permissions** and click **Add a permission**

![API permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/API_permissions.png)

8. Select **Microsoft Graph > Delegated permissions** and then select all necessary permissions in the application (_offline_access, User.Read, Mail.ReadWrite, Mail.Send_). Then click **Add permissions** to complete.

(\*) Note: Some permissions need admin consent so unless you are granted permission by the admin or the account is the admin itself, some permissions will not work.

![Request API Permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Request_API_Permissions.png)

9. After adding the permissions, navigate to **Authentication** and click **Add a platform**.

![Authentication Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Authentication.png)

10. In **Configure platforms**, select **Web.**

![Configure Platforms Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Configure_Platforms.png)

11. Add <https://api.vertexgraph.com/adminapi/assets/oauthcode> in Redirect URIs in Configure Web. Then select Configure to create the platform.

![Configure Web Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Configure_Web.png)

12. Then a window for Web platform will be created. Add the following URL’s below the first one.

<https://api.vertexgraph.com/adminapi/assets/oauthcode>

<https://login.microsoftonline.com/common/oauth2/v2.0/token>

<http://localhost/msoutlook/auth> (only apply to local setup, public url is different)

<http://localhost/msoutlook/token> (only apply to local setup, public url is different)

![Platform Configurations Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Platform_Configurations.png)

13. Once that is done, navigate **Overview** and click on **Add a certificate or secret** on the right corner.

![Add Certificate or Secret Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/Add_Certificate_or_Secret.png)

14. Click on **New client secret**, add a description to the client secret and then click **Add.**

![New Client Secret Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSOutlook/images/New_Client_Secret.png)

15. Scroll down and you will see your App Credentials, you want to copy your **Client ID** (or Application ID) and **Client Secret** (Client Secret Value)

(\*) Note: _Multiple applications can use the same app registrations, what matters is that permissions are added and redirect URLs and secrets are known_

## Test Prompts

Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website. Once the Microsoft Teams Skill is properly installed, you can try the following example prompts on the AI Assistant:

 - Show me all emails from example@example.com within the last 2 days from my Outlook
 - Show me my most recent email from example@example.com
 - Delete all emails I received from example@example.com from my Outlook
 - Reply to email from example@example.com with subject ‘example subject’ with content ‘example reply content.’
 - Forward the email from example@example.com with subject 'example subject’ to example2@example.com with additional content ‘forward email content example’
 - Send an email to example@example.com saying that ‘example send content’
