import { FileSystemEntry as FSE } from '@unmango/safir-protos/dist/agent/files_pb';

interface Props {
  entry: FSE;
}

const FileSystemEntry: React.FC<Props> = ({ entry }) => {
  const path = entry.getPath();

  return <span>{path}</span>;
};

export default FileSystemEntry;
