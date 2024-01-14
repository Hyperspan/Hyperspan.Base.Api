import { faHomeAlt, faPlus, faShareAlt, faStar, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useDispatch } from "react-redux";
import { addLoader, toggleUploadDialog } from "../../redux/DriveReducer";
import { useEffect, useState } from "react";
import { GetDriveInfo } from "../../services/FileDriveService";

export default function Sidebar() {
  const dispatch = useDispatch();
  const [driveInfo, setDriveInfo] = useState<{ freeSpace: number; totalSize: number } | null>(null);
  const [usedSpacePercentage, setUsedSpacePercentage] = useState(0);

  useEffect(() => {
    dispatch(addLoader([true]));
    GetDriveInfo()
      .then((x) => setDriveInfo(x.data))
      .finally(() => dispatch(addLoader([])));
  }, []);

  useEffect(() => {
    if (!driveInfo) return;
    setUsedSpacePercentage(((driveInfo.totalSize - driveInfo.freeSpace) / driveInfo.totalSize) * 100);
  }, [driveInfo]);

  return (
    <div className="sidebar">
      <ul className="nav flex-column">
        <li className="nav-item">
          <button onClick={() => dispatch(toggleUploadDialog(true))} className="btn btn-sm btn-light nav-link">
            <FontAwesomeIcon icon={faPlus} />
            <span>New</span>
          </button>
        </li>
        <hr className="dropdown-divider" />
        <li className="nav-item">
          <a className="nav-link active" aria-current="page" href="#">
            <FontAwesomeIcon icon={faHomeAlt} />
            <span>Home</span>
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link" href="#">
            <FontAwesomeIcon icon={faShareAlt} />
            <span>Shared</span>
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link" href="#">
            <FontAwesomeIcon icon={faStar} />
            <span>Starred</span>
          </a>
        </li>
        <li className="nav-item">
          <a className="nav-link" aria-disabled="true">
            <FontAwesomeIcon icon={faTrashAlt} />
            <span>Trash</span>
          </a>
        </li>
        <li className="nav-item d-flex flex-column align-items-baseline">
          <div className="progress">
            <div className="progress-bar" style={{ width: `${usedSpacePercentage.toFixed(2)}%` }}></div>
          </div>
          <p className="mt-2">
            Used {driveInfo && SizeModifier(driveInfo.totalSize - driveInfo.freeSpace)} of {driveInfo && SizeModifier(driveInfo.totalSize)}
          </p>
        </li>
      </ul>
    </div>
  );
}

export function SizeModifier(bytes: number) {
  const suffixes = ["B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
  let i = 0;

  while (bytes >= 1024 && i < suffixes.length - 1) {
    bytes /= 1024;
    i++;
  }

  return `${bytes.toFixed(2)} ${suffixes[i]}`;
}
