// Importing the useRouteError hook from react-router-dom to handle errors within routes.
import {useRouteError} from "react-router-dom";


interface RouteError {
    data: string;
    error: {
        columnNumber: number;
        fileName: string;
        lineNumber: number;
        message: string;
        stack: string;
    };
    internal: boolean;
    status: number;
    statusText: string;
    message?: string;
}

// Defining the ErrorPage component. This component is exported for use elsewhere in the application.
export default function ErrorPage() {
    // Using the useRouteError hook to obtain error details from the current route context.
    const error = useRouteError() as RouteError;

    // Logging the error to the console for debugging purposes.
    console.error(error);

    // The component returns a simple UI to display when an error occurs in routing.
    // It shows a generic message to the user along with the specific error message.
    return (
        <div id="error-page">
            <h1>Oops!</h1> {/* Title indicating an error has occurred */}
            <p>Sorry, an unexpected error has occurred.</p> {/* A general apology message to the user */}
            <p>
                {/* Displaying the error's status text if available, otherwise, its message. This provides specific info about the error. */}
                <i>{error.statusText || error.message}</i>
            </p>
        </div>
    );
}