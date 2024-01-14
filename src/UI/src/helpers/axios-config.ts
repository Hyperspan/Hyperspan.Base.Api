import axios, { AxiosError, AxiosResponse } from "axios";
import { ApiResponse } from "../responses/ApiResponse";
import { GetToken, RemoveToken } from "../services/UserService";
export const baseUrl = import.meta.env.APP_API_URL;
export const baseApiUrl = baseUrl + "/api";

export const Axios = () => {
    const axiosInstance = axios.create({
        baseURL: baseApiUrl,
    });

    const token = GetToken();

    if (token) {
        axiosInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    }

    axiosInstance.interceptors.response.use(
        (data: AxiosResponse<ApiResponse<any>>) => {
            return data;
        },
        (error: AxiosError) => {

            if (!error.response) {
                throw new AxiosError(error.message, error.code, error.config, error.request, error.response);
            }
            if (error.response.status === 400) {
                let errorList = (<any>error.response.data).errors;
                let errors: string[] = [];
                for (let index = 0; index < Object.keys(errorList).length; index++) {
                    const element = Object.keys(errorList)[index];
                    let errorsMap = [...errorList[element]].map((x) => separatePascalCamelCase(element) + ": " + x);
                    errors.push(...errorsMap);
                }
            }

            if ([401].includes(error.response.status)) {
                RemoveToken();
                window.location.replace("/accounts/login");
            }

            throw new AxiosError(error.message, error.code, error.config, error.request, error.response);
        },
    );

    return axiosInstance;
};

export function separatePascalCamelCase(input: string): string {
    const regex = /(?<=[a-z])(?=[A-Z])/;
    let word = "";
    input.split(regex).forEach((x) => {
        return (word += ` ${x.charAt(0).toUpperCase() + x.slice(1)}`);
    });
    return word;
}