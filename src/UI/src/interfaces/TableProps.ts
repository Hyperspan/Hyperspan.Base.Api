import { ReactNode } from "react"

export type TableProps<T = any> = {
    dataRows: T[]
    headers: TableHeaderProps<T>[]
    pagination: Pagination
}


export type TableHeaderProps<T = any> = {
    field: keyof T
    header?: string
    sortable?: boolean
    filter?: boolean
    onCellRender?: (props: T) => ReactNode | string
    onCellClick?: (props: any) => any
}

export type Pagination = {
    totalRecords: number
    currentPage: number
    recordsPerPage: number
}