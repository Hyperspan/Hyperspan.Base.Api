export interface ApiResponse<T = any> {
    errorCode: string;
    data: T | undefined;
    message: string[];
    succeeded: boolean
}