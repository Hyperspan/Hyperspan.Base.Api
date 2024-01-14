import { AxiosError } from "axios";
import { Axios } from "./axios-config";
import { ApiResponse } from "../responses/ApiResponse";

export function Get<T = any>(url: string, cancelToken: any = null, params: any = null): Promise<ApiResponse<T>> {
    return Axios()
        .get<ApiResponse<T>>(url, { cancelToken, params })
        .then((x) => {
            return x.data;
        })
        .catch((x) => {
            throw new AxiosError(x.message, x.code, x.config, x.request, x.response);
        });
}

export function Post<T = any, D = any>(url: string, data: D, cancelToken: any = null): Promise<ApiResponse<T>> {
    return Axios()
        .post<ApiResponse<T>>(url, data, { cancelToken })
        .then((x) => {
            return x.data;
        })
        .catch((x) => {
            throw new AxiosError(x.message, x.code, x.config, x.request, x.response);
        });
}

export function Put<T, D>(url: string, data: D, cancelToken: any = null): Promise<ApiResponse<T>> {
    return Axios()
        .put<ApiResponse<T>>(url, data, { cancelToken })
        .then((x) => {
            return x.data;
        })
        .catch((x) => {
            throw new AxiosError(x.message, x.code, x.config, x.request, x.response);
        });
}

export function Delete<T>(url: string, cancelToken: any = null): Promise<ApiResponse<T>> {
    return Axios()
        .delete<ApiResponse<T>>(url, { cancelToken })
        .then((x) => {
            return x.data;
        })
        .catch((x) => {
            throw new AxiosError(x.message, x.code, x.config, x.request, x.response);
        });
}

export function HttpRequest<T, D>(url: string, method: string, data?: D, cancelToken: any = null): Promise<ApiResponse<T>> {
    const config = {
        method: method,
        url: url,
        data: data,
        cancelToken,
    };

    return Axios()
        .request(config)
        .then((x) => {
            return x.data;
        })
        .catch((x) => {
            throw new AxiosError(x.message, x.code, x.config, x.request, x.response);
        });
}