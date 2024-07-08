import { useEffect } from "react"

interface ErrorPopupProps {
  msg: string;
  onClose: () => void;
}

//heavy work in progress....
export const ErrorPopup: React.FC<ErrorPopupProps> = ({ msg, onClose }) => {
  useEffect(() => {
    const closeAfter = 30 * 1000;
    const timer = setTimeout(() => {
      onClose();
    }, closeAfter);

    return () => {
      clearTimeout(timer);
    };
  }, [onClose]);

  return (
    <div className="text-sm border bg-indigo-700">
      <h1 className="font-bold">
        An error has occured: 
      </h1>
      <p className="font-semibold">
        {msg}
      </p>
    </div>
  );
};
