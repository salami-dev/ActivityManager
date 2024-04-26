import {IActivity} from "../Activity/interfaces";

export interface ITask {
    id: number;
    name: string;
    content: string;
    startDate: Date;
    endDate: Date;
    activity?: IActivity;

    dateTime?: (any | null)[] | undefined;
    created?: Date | null;
    status?: string | undefined;
    tags?: string[] | undefined;
}