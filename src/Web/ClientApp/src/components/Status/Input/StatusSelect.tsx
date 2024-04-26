import React, {useEffect, useState} from 'react';
import {Select, Tag} from 'antd';
import debounce from 'lodash/debounce';
import {StatusClient} from "../../../web-api-client";
import ThemedOption from "../../Utils/ThemedOption";
import {IOptionData, ICustomSelectProps} from "../../../globals/interfaces";
import {transformDataToOptions} from "../../Utils/helpers";

const {Option} = Select;


// Fetch data from API
const fetchData = async (inputValue: string | null, pageNumber: number = 1, pageSize: number = 5) => {
    try {
        const client = new StatusClient();
        const data = await client.getStatusWithPagination(inputValue, pageNumber, pageSize);
        return data.items ?? [];
    } catch (error) {
        console.error('Error fetching data:', error);
        return [];
    }
};

const StatusSelect: React.FC<ICustomSelectProps> = ({value, onChange}) => {
    const [options, setOptions] = useState<IOptionData[]>([]);
    // const [value, setValue] = useState<string | undefined>(undefined);


    const tagRender = (props: any) => {
        const {label, value, closable, onClose} = props;
        const color = options.find(option => option.value === value)?.theme;
        const tagStyle: React.CSSProperties = {
            backgroundColor: color || '#FFF', // Default to white if no color is found
            color: '#FFF', // You may want to adjust color based on the background for better readability
            borderRadius: '4px',
        };

        return (
            <Tag color={color} closable={closable} onClose={onClose} style={tagStyle}>
                {label}
            </Tag>
        );
    };

    const optionRender = (props: any) => {
        const {label, value, closable, onClose} = props;
        const color = options.find(option => option.value === value)?.theme;

        return (
            <ThemedOption label={label} value={value} bgColor={color}/>
            // <Tag color={color} closable={closable} onClose={onClose} style={tagStyle}>
            //     {label}
            // </Tag>
        );
    };

    const _fetchOptions = async (inputValue: string | null) => {
        const items = await fetchData(inputValue);
        const _options: IOptionData[] = transformDataToOptions(items);
        setOptions(_options);

        return _options;
    }

    // Define the fetchOptions function with debouncing
    const fetchOptions = debounce(async (inputValue: string) => {
        if (inputValue) {

            let _options = await _fetchOptions(inputValue);

            _options.unshift({value: inputValue, text: inputValue})
            setOptions(_options);

        } else {
            await _fetchOptions(null);
        }
    }, 500);

    useEffect(() => {
        (async () => {
            try {
                await _fetchOptions(null);

            } catch (error) {
                console.error('Error fetching data:', error);
            }
        })();

    }, []); // The empty array ensures this effect only runs once after the initial render

    const handleChange = (newValue: string) => {
        console.log("Selected:", newValue);
        onChange?.(newValue);
    };

    const handleSearch = (inputValue: string) => {
        fetchOptions(inputValue);
    };

    return (
        <Select
            showSearch
            value={value}
            placeholder="Type to search or enter custom value"
            defaultActiveFirstOption={false}
            filterOption={false}
            onSearch={handleSearch}
            onChange={handleChange}
            notFoundContent={null}
            style={{width: '100%'}}
            labelRender={tagRender}
            optionRender={optionRender}
            allowClear
        >
            {options.map((option: IOptionData) => (
                <Option key={option.value} value={option.value}>
                    {option.text}
                </Option>
            ))}
        </Select>
    );
};

export default StatusSelect;
