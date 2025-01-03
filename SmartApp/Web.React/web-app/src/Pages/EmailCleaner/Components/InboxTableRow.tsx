import React from "react";
import {
  EmailClassificationInformation,
  SpamClassificationEnum,
} from "../Types/table";
import { useI18n } from "src/_hooks/useI18n";
import {
  IconButton,
  Menu,
  MenuItem,
  TableCell,
  TableRow,
  Tooltip,
  Typography,
} from "@mui/material";
import { MoreVertRounded } from "@material-ui/icons";
import { SpamReport } from "../Types/emailCleanupTypes";
import moment from "moment";

interface IProps {
  data: EmailClassificationInformation;
  isBlocked: boolean;
  handleUpdateBlacklist: (fromAddress: string) => Promise<void>;
  handleReportAiTrainingData: (reportModel: SpamReport) => Promise<void>;
}

const InboxTableRow: React.FC<IProps> = (props) => {
  const { data, isBlocked, handleUpdateBlacklist, handleReportAiTrainingData } =
    props;
  const { getResource } = useI18n();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleOpenMenu = React.useCallback(
    (event: React.MouseEvent<HTMLElement>) => {
      setAnchorEl(event.currentTarget);
    },
    []
  );

  const handleCloseMenu = React.useCallback(async () => {
    setAnchorEl(null);
  }, []);

  const handleSubmitAndCloseMenu = React.useCallback(
    async (value: SpamClassificationEnum) => {
      await handleReportAiTrainingData({
        from: data.from,
        subject: data.subject,
        classification: value,
      });

      setAnchorEl(null);
    },
    [data.from, data.subject, handleReportAiTrainingData]
  );

  const handleBlocklist = React.useCallback(
    async (from: string) => {
      await handleUpdateBlacklist(from).then(() => {
        handleCloseMenu();
      });
    },
    [handleUpdateBlacklist, handleCloseMenu]
  );

  const aiPredictionLabel = React.useMemo(() => {
    switch (data?.classification?.classification) {
      case SpamClassificationEnum.Ham:
        return getResource("settings.labelHam");
      case SpamClassificationEnum.Spam:
        return getResource("settings.labelSpam");
      default:
        return getResource("settings.labelUnknown");
    }
  }, [data, getResource]);

  return (
    <TableRow>
      <TableCell>
        <Typography>{moment(data.messageDate).format("DD.MM.YYYY")}</Typography>
      </TableCell>
      <TableCell>
        <Typography>{data.from}</Typography>
      </TableCell>
      <TableCell>
        <Typography>{data.subject}</Typography>
      </TableCell>
      <TableCell>
        <Tooltip
          sx={{ display: "flex", justifyContent: "center" }}
          title={data.classification?.score}
          children={<Typography>{aiPredictionLabel}</Typography>}
        />
      </TableCell>
      <TableCell sx={{ display: "flex", justifyContent: "center" }}>
        <Tooltip
          placement="top"
          title={getResource("settings.labelSendAiTrainingData")}
          children={
            <IconButton size="small" onClick={handleOpenMenu}>
              <MoreVertRounded />
            </IconButton>
          }
        />
      </TableCell>
      <Menu open={open} anchorEl={anchorEl} onClose={handleCloseMenu}>
        <MenuItem onClick={handleBlocklist.bind(null, data.from)}>
          {getResource(
            isBlocked
              ? "settings.labelRemoveFromBlockList"
              : "settings.labelAddToBlockList"
          )}
        </MenuItem>
        <MenuItem
          onClick={handleSubmitAndCloseMenu.bind(
            null,
            SpamClassificationEnum.Ham
          )}
        >
          {getResource("settings.labelReportAsHam")}
        </MenuItem>
        <MenuItem
          onClick={handleSubmitAndCloseMenu.bind(
            null,
            SpamClassificationEnum.Spam
          )}
        >
          {getResource("settings.labelReportAsSpam")}
        </MenuItem>
        <MenuItem
          onClick={handleSubmitAndCloseMenu.bind(
            null,
            SpamClassificationEnum.Unknown
          )}
        >
          {getResource("settings.labelReportAsUnknown")}
        </MenuItem>
      </Menu>
    </TableRow>
  );
};

export default InboxTableRow;
