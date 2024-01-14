import { useState } from "react";

export const DragAndDrop = (props: DragAndDropProps) => {
  const [dragging, setDragging] = useState(false);

  const handleDragEnter = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setDragging(true);
  };

  const handleDragLeave = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setDragging(false);
  };

  const handleDragOver = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
  };

  const handleDrop = (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setDragging(false);
    const files = e.dataTransfer.files;
    handleFiles(files);
  };

  const handleFiles = (files: FileList | null) => props.setFiles(files);

  return (
    <div
      className={`drop-zone ${dragging ? "dragging" : ""}`}
      onDrop={handleDrop}
      onDragOver={handleDragOver}
      onDragEnter={handleDragEnter}
      onDragLeave={handleDragLeave}
      style={{ height: "30vh" }}
    >
      {props.files && Array(props.files.length).map((_, index) => <p>{props.files?.[index].name}</p>)}
      <p>Drag &amp; Drop files here</p>
      <input type="file" className="form-control mt-5 w-100" onChange={(e) => handleFiles(e.target.files)} />
    </div>
  );
};

export type DragAndDropProps = {
  setFiles: (e: FileList | null) => any;
  files: FileList | null;
};
