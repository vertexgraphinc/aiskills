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

## Test Prompts

Now that the Google Calendar Skill is properly installed, you can try the following example prompts:

 - "Give me all events happening within today"
 - "Create an event called 'testing event' starting at 6pm and ending at 8pm today and add example@example.com as an attendee
 - "Remove all events from my calendar within next week"
 - "Get instances of recurring events within the last 2 days"
 - "Remove my meeting event with example@example.com"
 - "Remove John Doe from the list of attendees of the event 'event example'"
 - Create a new calendar event
