import { MediaItem } from '@unmango/safir-protos/dist/manager';

interface Props {
  item: MediaItem;
}

const Media: React.FC<Props> = ({ item }) => {
  return (
    <span>
      Host: {item.getHost()} Path: {item.getPath()}
    </span>
  );
};

export default Media;
