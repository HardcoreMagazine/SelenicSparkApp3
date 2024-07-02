import { format, parseISO } from 'date-fns'

export function formatDtString(dts: string): string {
  const parsedDt = parseISO(dts);
  return format(parsedDt, "dd.MM.yyyy HH:mm 'UTC'")
}
