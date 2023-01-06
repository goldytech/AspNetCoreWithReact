import {TokenRequestModel} from "../types/tokenRequestModel";
import {useMutation} from "react-query";
import axios, {AxiosError, AxiosResponse} from "axios";
import Problem from "../types/problem";
import {useEffect, useState} from "react";

const useAccessToken = () => {
    const storedToken = localStorage.getItem('token');
    const [token, setToken] = useState(storedToken);
    const mutation = useMutation<AxiosResponse,AxiosError<Problem>,TokenRequestModel>(
        (tokenRequestModel:TokenRequestModel) => axios.post('http://localhost:3502/v1.0/invoke/auth-api/method/Auth/token',tokenRequestModel),
        { onSuccess: (response:AxiosResponse) => {
                setToken(response.data.accessToken);
                console.log("Api response: ", response.data);
                console.log("Access token received" + response.data.accessToken);
                localStorage.setItem('token', response.data.accessToken);
            }}

    )
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            console.log("Access token retrieved from local storage: " + token);
            setToken(token);
        }
    }, [])
    return {token, mutation};
}
export default useAccessToken;
// interface TokenResponse {
//     AccessToken : string
// }
//
// export const useGetToken = () => {
//     let mutate, { data, error, isLoading } = useMutation<
//         TokenResponse,
//         Error,
//         TokenRequestModel
//     >(async (requestBody: TokenRequestModel) => {
//         const token = localStorage.getItem('token');
//
//         if (token) {
//             return token;
//         }
//
//         const response = await axios.post<TokenResponse>(
//             'https://your-token-endpoint.com',
//             requestBody
//         );
//
//         localStorage.setItem('token', response.data.AccessToken);
//
//         return response.data.AccessToken;
//     });
//
//     return {
//         getToken: mutate,
//         data,
//         error,
//         isLoading,
//     };
// };