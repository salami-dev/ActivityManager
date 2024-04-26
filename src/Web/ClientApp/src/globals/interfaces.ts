import {FormInstance} from "antd";
import React from "react";

export interface IOptionData {
    value: string;
    text: string;
    theme?: string;
}

export interface ICustomSelectProps {
    value?: string;  // selected value
    onChange?: (value: string) => void;  // function to handle changes
}


export interface IBaseFormRef {
    submitForm: () => void;
    resetForm?: () => void;
}

export interface ICreateFormRef extends IBaseFormRef {

}

export interface IEditFormRef extends IBaseFormRef {

}

export interface IBaseFormProps {
    loading?: boolean;
    onFinished?: (values: any) => void;
    initialValues?: object;
}

export interface ICreateFormProps extends IBaseFormProps {
    onFinished?: () => void;
    setLoading?: (value: boolean) => void;
}

export interface IEditFormProps extends ICreateFormProps {
    editValue: number | undefined
}

export interface IFormType {
    type: 'create' | 'edit';
    editValue?: number
}

export interface IFilterProps {
    searchTerm?: string | null
    activityId?: number
}

export interface IPageListFilterProps {
    filterOptions: IFilterProps
    onSearch: (value: IFilterProps) => void;
    form: FormInstance;
    showForm: boolean;
    toggleForm: (value: boolean) => void;
    buttonText: string
    loading: boolean;
    children?: React.ReactNode;


}