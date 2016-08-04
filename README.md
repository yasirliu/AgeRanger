AgeRanger is a world leading application designed to identify person's age group!
The only problem with it is... It is not implemented.

To help AgeRanger to conquer the world please implement a web application that does the following:

 - Allows user to add a new person - every person has a first name, last name and age;
 - Displays a list of people in the DB with their First and Last names, age and their age group. The age group should determened based on the AgeGroup DB table - a person belongs to the age group where person's age >= 
 than group's MinAge and < than group's MaxAge. Please note that MinAge and MaxAge can be null;
 - Allows user to search for a person by his/her first or last name and display all the relevant information for the person - first and last names, age, age group.

In our fantasies AgeRanger is a single page application, and our DBA has already implied that he wants us to migrate it to SQL Server some time in the future.
And unit tests! We love unit tests!

Last, but not he least - our sales manager suggests you'll get boonus points if the application will also allow user to edit existing person records and expose a WEB API.

You are free to use any technology and frameworks you need. However if you decide to go with third party package manager or dev tool - don't forget to mention them in the README.md of your fork.

Good luck!