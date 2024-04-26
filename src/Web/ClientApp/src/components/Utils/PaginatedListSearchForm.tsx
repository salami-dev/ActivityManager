import React from "react";
import {Form, Input} from "antd";
import {IFilterProps} from "../../globals/interfaces";

interface IPaginatedListSearchFormProps {
    value: IFilterProps;
    onChange: (props: IFilterProps) => void;
    loading?: boolean;
    children?: React.ReactNode;
}

const {Search} = Input;
const PaginatedListSearchForm: React.FC<IPaginatedListSearchFormProps> = ({value, onChange, loading, children}) => {

    const changeHandler = async (value: string, event: any, info: any) => {
        const source = {info};
        await onChange({searchTerm: value});
    }
    return (
        <Form>
            <Form.Item
                name="search">
                <Search placeholder="Search ..." value={value.searchTerm ?? ""} loading={loading} enterButton
                        allowClear
                        onSearch={changeHandler}/>
            </Form.Item>
            {children}
        </Form>
    )

}

export default PaginatedListSearchForm;