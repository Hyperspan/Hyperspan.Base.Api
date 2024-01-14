import { TableProps } from "../../interfaces/TableProps";

export default function Table<T>(props: TableProps<T>) {
  return (
    <>
      <table className="table">
        <thead>
          <tr>
            {props.headers.map((header, index) => (
              <th key={index}>{header.header ?? header.field.toString()}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {props.dataRows.map((row, index) => (
            <tr key={index}>
              {props.headers.map((header, index) => (
                <td key={index}>
                  {header.onCellRender && header.onCellRender(row)}
                  {!header.onCellRender && (row as any)[header.field]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}
