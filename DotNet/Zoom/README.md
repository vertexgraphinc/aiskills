# Zoom Skill

This skill allows the authenticated user to perform simple tasks on his/her Zoom account directly from the VertexGraph chat prompt.

## API Permissions Setup

1. Go to <https://marketplace.zoom.us>
2. Login with your Zoom account credentials.
3. Go to **Develop > Build App.**

![Zoom MarketPlace Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_MarketPlace.png)

4. After the app finishes creating, navigate to **Basic Information**. Select **User-managed** and copy down your **ClientID** and **ClientSecret**. In **OAuth Information**, add this link <https://api.vertexgraph.com/adminapi/assets/oauthcode> to the **OAuth RedirectURL**. Then add the following links into OAuth Allow Lists:

<https://api.vertexgraph.com/adminapi/assets/oauthcode>

<http://localhost/zoom/auth> (only apply to local setup, public url is different)

<http://localhost/zoom/token> (only apply to local setup, public url is different)

![Application Basic Info Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_App_Basic_Info.png)

5. Navigate to **Scope** and add all necessary permissions in the application (_user:read:user, meeting:read:list_meetings, meeting:write:meeting, meeting:update:meeting, meeting:delete:meeting, user:read:list_recordings_). Then click **Continue** to be ready for using.

![API Scope Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_Scope.png)

6. (Note: This step is only done while obtaining the authorization code for the Vertexgraph app) While obtaining the authorization code, click on **Add App Now** and authorize the app with the stated scopes/permissions.

![API Scope Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_Authentication.png)

![API Scope Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_Consent.png)

## Test Prompts

Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website. Once the Microsoft Outlook Skill is properly installed, you can try the following example prompts on the AI Assistant:

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