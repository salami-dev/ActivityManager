import React from 'react';
import {format, parseISO, Locale} from 'date-fns';
import {toZonedTime} from 'date-fns-tz';

interface FormattedDateProps {
    date: Date | null;
    formatString?: string; // Optional, provide a default format
}

// Assuming the structure of each locale module,
// and that `date-fns/locale` exports them as named exports.
interface LocaleModules {
    [key: string]: Locale;
}

let allLocales: LocaleModules;

// Dynamically importing all locales
import("date-fns/locale").then(locales => {
    allLocales = locales as unknown as LocaleModules;
});

// Helper function to retrieve the correct locale object
const getLocale = (): Locale | undefined => {
    const locale = navigator.language.replace("-", "");
    const rootLocale = locale.substring(0, 2);

    return allLocales?.[locale] || allLocales?.[rootLocale];
};


const FormattedDate: React.FC<FormattedDateProps> = ({date, formatString = "PPPPpp"}) => {
    try {

        if (!date) {
            throw new Error("Invalid Date");
        }

        const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone; // Automatically detect the user's timezone
        const locale = getLocale(); // Automatically detect the user's locale
        const zonedDate = toZonedTime(date, timeZone); // Convert the UTC date to the user's timezone

        return (
            <span>
                {format(zonedDate, formatString, {locale})}
            </span>
        );
    } catch (error) {
        console.error('Invalid date format', error);
        return <span>Invalid date</span>; // Handling parsing errors
    }
};

export default FormattedDate;
