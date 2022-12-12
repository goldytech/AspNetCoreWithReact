import {Customer} from "../types/customer";
import axios, {AxiosError} from "axios";
import {useQuery} from "react-query";

const useFetchCustomers = () => {
    return useQuery<Customer[],AxiosError>('customers', () => {
        return axios.get<Customer[]>('api/customers')
            .then(response => response.data)
    })
}
export default useFetchCustomers;