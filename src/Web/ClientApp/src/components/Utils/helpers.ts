import {IOptionData} from "../../globals/interfaces";

// Data transformation helper
export const transformDataToOptions = (items: any[], useIdAsValue: boolean = false): IOptionData[] => {
    return items.map(item => {
        const option: IOptionData = {value: item.name ?? "", text: item.name ?? ""};
        if (item.theme) {
            option.theme = item.theme ?? "";
        }
        if (useIdAsValue) {
            option.value = item.id
        }
        return option;
    });
};

type GenericObject = { [key: string]: any };

export const cleanObject = (obj: GenericObject): GenericObject => {
    return Object.keys(obj).reduce((acc: GenericObject, key: string) => {
        // Check if the value is not null
        if (obj[key] !== null) {
            acc[key] = obj[key];
        }
        return acc;
    }, {});
};