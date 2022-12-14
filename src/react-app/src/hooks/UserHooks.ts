import axios, {AxiosError, AxiosResponse} from "axios";
import {useMutation, useQuery, useQueryClient} from "react-query";
import { Claim } from "../types/claim";
import Problem from "../types/problem";
import {LoginModel} from "../types/loginModel";

const useFetchUser = () => {
    return useQuery<Claim[], AxiosError>("user", () =>
        axios
            .get('/account/getuser?slide=false')
            .then((resp) => resp.data)
    );
};

const useLogin = () => {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse,AxiosError<Problem>, LoginModel>(
        (loginModel) => axios.post('http://localhost:5135/Auth/login', loginModel),
        {
            onSuccess: () => {
                console.log('Login successful');
                queryClient.invalidateQueries('user');
            },
            onError: (error) => {
                console.log(error);
            }
        }
    )
}

export  {useLogin, useFetchUser};
