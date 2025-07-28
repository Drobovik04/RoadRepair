import dayjs from "dayjs";

export function normalize(values: Record<string, any>, fields: string[]) {
  const normalized: Record<string, any> = { ...values };

  for (const field of fields) {
    normalized[field] = values[field] ? dayjs(values[field]) : null;
  }

  return normalized;
}

/**
 * Преобразует поля в строку формата "YYYY-MM-DD"
 */
export function denormalize(values: Record<string, any>, fields: string[]) {
  const denormalized: Record<string, any> = { ...values };

  for (const field of fields) {
    denormalized[field] = values[field]
      ? dayjs(values[field]).format("YYYY-MM-DD")
      : null;
  }

  return denormalized;
}
