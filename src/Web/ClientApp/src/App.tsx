// Importing routing functionalities from the react-router-dom package
import {createBrowserRouter} from "react-router-dom";

import Root from "./routes/Root";
import ErrorPage from "./routes/Error";

// Importing components and functions from various route files
import Activities from "./routes/Activities";
import Tasks from "./routes/Tasks";
import Dashboard from "./routes/Dashboard";
import Statuses from "./routes/Statuses";

// Creating a router instance using createBrowserRouter. This configures the routes for the application.
const router = createBrowserRouter([
    {
        path: "/", // The root path
        element: <Root/>, // The component that will be rendered at the root path
        errorElement: <ErrorPage/>, // The component that will be rendered in case of an error at this route
        // loader: rootLoader, // The loader function for fetching initial data needed by the root route
        // action: rootAction, // The action function for handling form submissions at the root route
        children: [
            // Nested routes under the root path
            {
                errorElement: <ErrorPage/>, // Error component for nested routes
                children: [
                    {index: true, element: <Dashboard/>}, // The default child route of the root path
                    {
                        path: "activities", // Path for individual contact details, with a dynamic segment for the contact ID
                        element: <Activities/>, // Component to render for this path
                        // loader: contactLoader, // Loader function for fetching data specific to a contact
                        // action: contactAction, // Action function for this route
                    },
                    {
                        path: "tasks", //
                        element: <Tasks/>, // Component to render for this path

                    },
                    {
                        path: "statuses", //
                        element: <Statuses/>, // Component to render for this path

                    },

                    // {
                    //     path: "contacts/:contactId/edit", // Path for editing a contact
                    //     element: <EditContact/>, // Component to render for the edit page
                    //     loader: contactLoader, // Reusing the contact loader for fetching data needed for editing
                    //     action: editAction, // Action function for handling the edit form submission
                    // },
                    // {
                    //     path: "contacts/:contactId/destroy", // Path for deleting a contact
                    //     action: destroyAction, // Action function for handling contact deletion
                    //     errorElement: <div>Oops! There was an error.</div>, // Error component for this specific route
                    // },
                ],
            }
        ],
    },
]);

export default router;