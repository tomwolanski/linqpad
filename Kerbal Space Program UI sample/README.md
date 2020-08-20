Sample files for ['Not-so-hidden features of LINQPad'](https://www.linkedin.com/pulse/not-so-hidden-features-linqpad-tomasz-wola%C5%84ski) article.

### To create connection with db file:
- click "Add Connection" in left top part of the window
- use the default driver (LINQ to SQL), click "Next"
- specify "(localdb)\MSSQLLocalDB" (or any other existing SQL server) as a server
- in database section select option "Attach database file" and browse for .mdf file, click "OK"

### To run sample against the database:
- open .linq file
- in top of the window, in "Connection" dropdown select desired target database (you can also drag database form tree view and drop it into the query text)
- F5 or "Execute" button

![UI preview](https://github.com/tomwolanski/linqpad/raw/master/Kerbal%20Space%20Program%20UI%20sample/ksp_ui.gif)
