import { useEffect, useState } from "react";
import { ModalProps } from "../../interfaces/modal/ModalProps";
import { ColorTypes } from "../../interfaces/modal/ColorTypes";

export default function Modal(props: ModalProps) {
  const [show, setShow] = useState(props.show);
  useEffect(() => {
    props.setShow(show);
  }, [show]);

  return (
    <>
      <div className={`modal fade modal-${props.type ?? ColorTypes.secondary} ${show ? " show" : ""}`} style={{ display: show ? "block" : "none" }}>
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h6 className="modal-title">{props.title}</h6>
              <button type="button" className="btn-close" onClick={() => setShow(false)}></button>
            </div>
            <div className="modal-body">{props.children}</div>
            {props.footer && <div className="modal-footer">{props.footer}</div>}
          </div>
        </div>
      </div>
    </>
  );
}
