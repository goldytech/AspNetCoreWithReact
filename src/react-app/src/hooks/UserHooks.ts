import axios, { AxiosError } from "axios";
import { useQuery } from "react-query";
import { Claim } from "../types/claim";

const useFetchUser = () => {
    return useQuery<Claim[], AxiosError>("user", () =>
        axios
            .get('account/getuser?slide=false')
            .then((resp) => resp.data)
    );
};

export default useFetchUser;
