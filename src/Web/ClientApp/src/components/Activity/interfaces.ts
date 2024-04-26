import {ICustomSelectProps} from "../../globals/interfaces";

export interface IActivity {
    id: number;
    name: string;
    description: string;
    url: string | null;

    startDate?: Date | null;
    endDate?: Date | null;
    dateTime?: (any | null)[] | undefined;
    created?: Date | null;
    status?: string | undefined;
    activityType?: string | undefined;
    tags?: string[] | undefined;
}



