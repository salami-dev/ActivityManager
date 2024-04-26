import {Select} from "antd";

const {Option} = Select;
export const selectBefore = (
    <Select defaultValue="https://">
        <Option value="http://">http://</Option>
        <Option value="https://">https://</Option>
    </Select>
);