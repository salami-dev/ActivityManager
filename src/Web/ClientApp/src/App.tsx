// Importing routing functionalities from the react-router-dom package
import {createBrowserRouter} from "react-router-dom";

import Root from "./routes/Root";
import ErrorPage from "./routes/Error";

// Importing components and functions from various route files
import Activities from "./routes/Activities";
import Tasks from "./routes/Tasks";
import Dashboard from "./routes/Dashboard";
import Statuses from "./routes/Statuses";
import ActivityTypes from "./routes/ActivityTypes";
import Tags from "./routes/Tags";

// Creating a router instance using createBrowserRouter. This configures the routes for the application.
const router = createBrowserRouter([
    {
        path: "/", // The root path
        element: <Root/>, // The component that will be rendered at the root path
        errorElement: <ErrorPage/>, // The component that will be rendered in case of an error at this route
        children: [
            // Nested routes under the root path
            {
                errorElement: <ErrorPage/>, // Error component for nested routes
                children: [
                    {index: true, element: <Dashboard/>}, // The default child route of the root path
                    {
                        path: "activities", // Path for individual contact details, with a dynamic segment for the contact ID
                        element: <Activities/>, // Component to render for this path
                    },
                    {
                        path: "tasks", //
                        element: <Tasks/>, // Component to render for this path

                    },
                    {
                        path: "statuses", //
                        element: <Statuses/>, // Component to render for this path

                    },

                    {
                        path: "activitytypes", //
                        element: <ActivityTypes/>, // Component to render for this path

                    },

                    {
                        path: "tags", //
                        element: <Tags/>, // Component to render for this path

                    },

                ],
            }
        ],
    },
]);

export default router;