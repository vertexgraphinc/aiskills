**VertexGraph AI Skill – Microsoft Teams**

**Setup Microsoft App Permission:**

1. Go to <https://entra.microsoft.com>
2. Login with your Microsoft account credentials.
3. Go to **Applications > App registrations.**

![App Registrations Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/App_Registrations.png)

1. Select **New registrations**. Fill out the desired name for the registration and then choose second or third option in **Supported account types.** You can leave Redirect URI empty.

![A screenshot of a computer application

Description automatically generated](data:image/![Register an Application Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Register_an_Application.png)

1. Select **Register** to create a new app registration.
2. After that is done, navigate back to App registrations in step 3 and click on your newly create application.
3. Now navigate to **API Permissions** and click **Add a permission**

![API permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/API_permissions.png)

1. Select **Microsoft Graph > Delegated permissions** and then select all necessary permissions in the application (_offline_access, User.Read, User.ReadBasic.All, Chat.ReadWrite, ChatMember.ReadWrite, Group.ReadWrite.All, TeamMember.ReadWrite.All, Team.ReadBasic.All_). Then click **Add permissions** to complete.

(\*) Note: Some permissions need admin consent so unless you are granted permission by the admin or the account is the admin itself, some permissions will not work.

![Request API Permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Request_API_Permissions.png)

1. After adding the permissions, navigate to **Authentication** and click **Add a platform**.

![Authentication Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Authentication.png)

1. In **Configure platforms**, select **Web.**

![Configure Platforms Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Configure_Platforms.png)

1. Add <https://api.vertexgraph.com/adminapi/assets/oauthcode> in Redirect URIs in Configure Web. Then select Configure to create the platform.

![Configure Web Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Configure_Web.png)

1. Then a window for Web platform will be created. Add the following URL’s below the first one.

<https://api.vertexgraph.com/adminapi/assets/oauthcode>

<https://login.microsoftonline.com/common/oauth2/v2.0/token>

<http://localhost/msteams/auth>

<http://localhost/msteams/token>

![Platform Configurations Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Platform_Configurations.png)

1. Once that is done, navigate **Overview** and click on **Add a certificate or secret** on the right corner.

![Add Certificate or Secret Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/Add_Certificate_or_Secret.png)

1. Click on **New client secret**, add a description to the client secret and then click **Add.**

![New Client Secret Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/New_Client_Secret.png)

1. Scroll down and you will see your App Credentials, you want to copy your **Client ID** (or Application ID) and **Client Secret** (Client Secret Value)

Note: _Multiple applications can use the same app registrations, what matters is that permissions are added and redirect URLs and secrets are known_

**Setting up Skill in VertexGraph**

1. Navigate to <https://vertexgraph.ai/portal/>
2. On the left side tool bar select the second option “AI”
3. Now select the custom skills icon on the top right corner

![AI Assistants Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/MSTeams/images/AI_Assistants.png)

1. Once on the skills section Press “New Custom Skill”
2. Follow the setup until you reach the section where you must enter the OAuth Client ID and Secret
3. Copy and paste the client ID and secret from the MSTeams AI Skill information
4. Get the authorization code and authorize VertexGraph to access your Microsoft account
5. Once completed assign the new skill to any of your assistants to start using it

**Test Prompts:**

- Get chats for the past week.
- Create a chat with <example@example.com> in my MS teams.
- Update my most recent chat's topic to "chat with friend”.
- Add <testing1@vertexgraph.com> to my chats within last 2 days in my MS teams.
- Remove <example@example.com> from my chats within last 2 days in my MS teams.
- Get chat messages for the past week from my MS teams.
- Send a message with content "test message from ai chat" to chats within past week from my MS teams.
- Update chat messages to "new update message" from my MS teams in the chat with topic containing 'test1'.
- Remove chat messages from my MS teams with topic containing "chat".
- Get teams with <example@example.com>.
- Create a team with <example@example.com>.
- Update most recently created team display name to "update team display name”.
- Remove most recently created team.
- Get team members who are the owner of the teams.
- Add <example@example.com> to all teams.
- Remove <testing1@vertexgraph.com> from most recent team.