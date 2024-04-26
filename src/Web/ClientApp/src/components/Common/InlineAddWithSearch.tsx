import React from "react";

import {Button, Col, Flex, Row} from "antd";
import {CloseOutlined, PlusOutlined} from "@ant-design/icons";

import {IPageListFilterProps} from "../../globals/interfaces";
import PaginatedListSearchForm from "../Utils/PaginatedListSearchForm";


const InlineAddWithSearch: React.FC<IPageListFilterProps> = (props, context) => {

    const {filterOptions, onSearch, form, showForm, toggleForm, buttonText, loading, children} = props

    return (
        <Row>
            <Col span={12}>
                <PaginatedListSearchForm value={filterOptions} onChange={onSearch}/>
            </Col>

            <Col span={12}>
                <Flex justify="end">
                    <Button
                        type={!showForm ? "primary" : "default"}
                        {...(!showForm ? {icon: <PlusOutlined/>} : {icon: <CloseOutlined/>})}
                        onClick={() => {
                            form.resetFields()
                            toggleForm(!showForm)
                        }}
                        style={{marginBottom: 16}}
                        loading={loading}
                        disabled={loading}
                    >
                        {/*{!showForm ? "Add New Status" : "Cancel"}*/}
                        {buttonText}
                    </Button>
                </Flex>
            </Col>
        </Row>
    )
}

export default InlineAddWithSearch;