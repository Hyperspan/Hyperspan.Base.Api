import { ColorTypes } from "./ColorTypes";

export type ModalProps = {
    title: string;
    children: React.ReactNode | string;
    footer?: React.ReactNode | string;
    setShow: (show: boolean) => any;
    show: boolean;
    type?: ColorTypes;
};