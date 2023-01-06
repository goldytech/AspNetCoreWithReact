import axios, {AxiosError, AxiosResponse} from "axios";
import {useMutation, useQuery, useQueryClient} from "react-query";
import { Claim } from "../types/claim";
import Problem from "../types/problem";
import {LoginModel} from "../types/loginModel";
import {useNavigate} from "react-router-dom";

const useFetchUser = () => {
    return useQuery<Claim[], AxiosError>("user", () =>
        axios
            .get('http://localhost:5135/Auth/userinfo')
            .then((resp) => resp.data)
    );
};

const useLogin = () => {
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    return useMutation<AxiosResponse,AxiosError<Problem>, LoginModel>(
        // dapr Url : http://localhost:{daprPort}/v1.0/invoke/{daor-appid}/method/Auth/login
        // local Url : http://localhost:5135/Auth/login
        (loginModel) => axios.post('http://localhost:3502/v1.0/invoke/auth-api/method/Auth/login', loginModel),
        {
            onSuccess: () => {
                console.log('Login successful');
                queryClient.invalidateQueries('user');
                navigate('/customers');
            },
            onError: (error) => {
                console.log(error);
            }
        }
    )
}

export {useFetchUser, useLogin};
