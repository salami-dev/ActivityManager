import React, {useEffect, useState} from 'react';
import {Select} from 'antd';
import debounce from 'lodash/debounce';
import {ActivitiesClient} from "../../../web-api-client";
import {IOptionData, ICustomSelectProps} from "../../../globals/interfaces";
import {transformDataToOptions} from "../../Utils/helpers";

const {Option} = Select;


// Fetch data from API
const fetchData = async (inputValue: string | null, pageNumber: number = 1, pageSize: number = 5) => {
    try {
        const client = new ActivitiesClient();
        const data = await client.getActivitiesWithPagination(inputValue, pageNumber, pageSize);
        return data.items ?? [];
    } catch (error) {
        console.error('Error fetching data:', error);
        return [];
    }
};

const ActivitySelect: React.FC<ICustomSelectProps> = ({value, onChange}) => {
    const [options, setOptions] = useState<IOptionData[]>([]);

    const _fetchOptions = async (inputValue: string | null) => {
        const items = await fetchData(inputValue);
        const _options: IOptionData[] = transformDataToOptions(items, true);
        setOptions(_options);

        return _options;
    }

    // Define the fetchOptions function with debouncing
    const fetchOptions = debounce(async (inputValue: string) => {
        if (inputValue) {

            let _options = await _fetchOptions(inputValue);
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
            placeholder="Type to search"
            defaultActiveFirstOption={false}
            filterOption={false}
            onSearch={handleSearch}
            onChange={handleChange}
            // notFoundContent={null}
            style={{width: '100%'}}
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

export default ActivitySelect;
