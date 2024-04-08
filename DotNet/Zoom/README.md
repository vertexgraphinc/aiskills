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

5. Navigate to **Scope** and add all necessary permissions in the application (_user:read:user, meeting:read:list_meetings, meeting:write:meeting, meeting:update:meeting, meeting:delete:meeting, cloud_recording:read:list_recording_files_). Then click **Continue** to be ready for using.

![API Scope Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoom/images/Zoom_Scope.png)

## Test Prompts

Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website. Once the Zoom Skill is properly installed, you can try the following example prompts on the AI Assistant:

- Get my upcoming Zoom meetings
- Create a meeting in zoom at 5 pm with example@example.com and example2@example.com
- Update my upcoming Zoom meeting to 8pm the next day
- Remove my Zoom meetings from today till the end of the week
- Get all my Zoom recordings