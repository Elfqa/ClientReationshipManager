# ClientRelationshipManager

ClientRelationshipManager is a mini CRM application designed for bank Advisors to manage Clients relationships efficiently. It is built using ASP.NET WebAPI for the backend and React for the frontend, and it utilizes MS SQL Server as the database. The project code is divided into backend ([ClientReationshipManager](https://github.com/Elfqa/ClientReationshipManager)) and frontend ([CRMapp](https://github.com/Elfqa/CRMapp)) parts.

## Description

ClientRelationshipManager helps bank Advisors manage their portfolios of Clients. Advisors can schedule, view, edit and delete Contacts with Clients. Contacts include, for example, meetings and telephone calls. Additionally, Advisors can search for information about the Client and his meetings with other Advisors. The application is intended to enable Advisors to manage their work schedules and maintain effective communication with their Clients.

## Features

### ASP.NET WebAPI and MSSQLServer (Backend) 
- **Database Setup**: The application has a fully built database with tables for Advisors, Clients, ClientAdvisorRelationships and Contacts.
- **Sample Data**: The database is pre-populated with sample data.
- **User Authentication**: Advisor can log in to the system. JWT Authorization has been implemented.
- **View Contacts**: Advisor can fetch a list of their Contacts.
- **View Single Contact**: Advisor can fetch details of a single Contact.
- **Schedule Contact**: Advisor can schedule a new Contact with a Client. The Contact can be set for a future date and time, and it will receive a "scheduled" status.
- **Reschedule Contacts**: Advisor can reschedule planned Contacts.
- **Update Contact**: Advisor can change the status of a scheduled Contact to "completed" and add start and end dates for past Contacts.
- **Delete Contact**: Advisor can delete Contacts.
- **View Clients**: Advisor can fetch a list of their clients.
- **View Client by ID**: Advisor can fetch a single client by ID along with the assigned advisor.
- **Manage Clients**: Advisor can add, edit, and delete clients.
- **System Logs**: Store system logs in the database.

### React (Frontend)
- **User Authentication**: Advisor can log in to the system using a login form. JWT tokens are passed upon successful authentication.
- **Web Navigation**: Conditional rendering for logged in and unlogged user. Unlogged user may only display the login page.
- **View Clients**: Logged in Advisor displays Clients assigned to him on the dedicated page.
- **View Single Client**: Advisor can fetch details of a single Client.
- **Schedule Contact**: Advisor can schedule a new Contact with a Client thanks to a controlled form.
- **Reschedule Contact**: Advisor can edit scheduled Contacts thanks to a controlled form.
- **Update Contact**: Advisor can set the Contact as "completed" and add start and end dates thanks to a controlled form.
- **Delete Contact**: Advisor can delete Contacts.
- **View Contacts**: Advisor can view all scheduled and completed Contacts with clients. Scheduled and completed Contacts are displayed in a different color to make contact management easier.


#### To Be Developed
- **User Roles**: Add roles such as User for Advisor and Admin. Admins can edit User information and relationship between Clients and Advisors.
- **Different Contact Types**: Add various types of contacts (meeting, phone call).
- **Contact Categories**: Add categories for contacts (sales, relational, informational, etc.).
- **Additional Data Tables**: Add extra tables to the database with Client information (products, events) and display them on the Client card.
- **Contact Reminders**: Popup reminders for Contacts.
- **Auto-fill Contacts**: Auto-fill feature for Clients in the Contact creation form.
- **Page pagination**: Page pagination with display of contacts


## Installation

### Prerequisites
- .NET 6.0 SDK
- Node.js 14+
- MSSQLServer

### Backend Setup
1. Clone the repository:
    ```bash
    git clone https://github.com/Elfqa/ClientRelationshipManager.git
    ```
2. Restore dependencies and build the project:
    ```bash
    dotnet restore
    dotnet build
    ```
3. Update the `appsettings.json` file with your MSSQLServer connection string.
4. Run the application:
    ```bash
    dotnet run
    ```

### Frontend Setup
1. Clone the repository:
    ```bash
    git clone https://github.com/Elfqa/CRMapp.git
	   
2. Install the dependencies:
    ```bash
    npm install
    ```
3. Run the application:
    ```bash
    npm start
    ```


## Authors

- **Ewa Liniewska** - *Initial work* - [Elfqa](https://github.com/Elfqa)
