export type BreadCrumsProps = {
  path: string[];
};

export default function BreadCrums(props: BreadCrumsProps) {
  return (
    <>
      <div className="breadcrum-container">
        <ol className="breadcrum-list">
          {props.path.map((x, index) => (
            <li className={`breadcrum-item ${index + 1 === props.path.length ? "active" : ""}`} key={index}>
              {x}
            </li>
          ))}
        </ol>
      </div>
    </>
  );
}
