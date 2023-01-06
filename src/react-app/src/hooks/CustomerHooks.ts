import {Customer} from "../types/customer";
import axios, {AxiosError} from "axios";
import {useQuery} from "react-query";
import useAccessToken from "./TokenHooks";
import {TokenRequestModel} from "../types/tokenRequestModel";


const useFetchCustomers = () => {
    const {token,mutation} = useAccessToken();
    if (!token) {
        const tokenRequestModel : TokenRequestModel = {
            clientId: 'admin',
            clientSecret: '123',
            audience: 'bff'

        }
        mutation.mutate( tokenRequestModel);
    }
    return useQuery<Customer[],AxiosError>('customers', () => {
        console.log("Token getting passed in Customers API " + token);
        return axios.get<Customer[]>('http://localhost:3500/v1.0/invoke/bff/method/customers',{headers: {'Authorization': `Bearer ${token}`}})
            .then(response => response.data)
    })
}
export default useFetchCustomers;